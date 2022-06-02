import React, { useContext, useEffect, useState } from 'react'
import { Link } from 'react-router-dom'
import {
	createStyles,
	makeStyles,
	Box,
	Button,
	Checkbox,
	FormControlLabel,
	List,
	ListItem,
	Typography
} from '@material-ui/core'
import Pagination from '@material-ui/lab/Pagination'
import ArrowDownwardIcon from '@material-ui/icons/ArrowDownward'
import ArrowUpwardIcon from '@material-ui/icons/ArrowUpward'

import CommentTile from '../../card&tiles/CommentTile/CommentTile'
import CommentArea from '../../atoms/CommentArea/CommentArea'
import UserContext from '../../../context'
import OnPageCountSwitcher from '../../atoms/OnPageCountSwitcher/OnPageCountSwitcher'

const useStyles = makeStyles((theme) => createStyles({
	header: {
		display: 'flex',
		justifyContent: 'space-between',
	},
	leftMargin: {
		marginLeft: theme.spacing(2),
	},
	list: {
		width: '100%',
		height: '250px',
		overflowY: 'scroll',
	},
	listNavigation: {
		display: 'flex',
		alignContent: 'center',
		alignItems: 'center',
	},
}));

function CommentsList(props) {
	const classes = useStyles();
	const { user, _ } = useContext(UserContext);

	const [state, setState] = useState({
		onPage: 3,
		comments: [],
		loading: true,
		totalCount: 0,
		countPages: 0,
		currentPage: 1,
		sortByDateInCommonOrder: true,
		userCommentsFirst: false
	});

	const [reload, setReload] = useState(false);

	const calculatePagesCount = (total, onPage) => {
		if (total > onPage) {
			let value = Math.floor(total / onPage);
			return value * onPage === total ? value : value + 1;
		}
		else {
			return 1;
		}
	}

	useEffect(() => {
		fetch(
			'https://localhost:5001/api/Comment' +
			`?PageNumber=${state.currentPage}` +
			`&OnPageCount=${state.onPage}` +
			`&FilmId=${props.filmId}` +
			`&CommonOrder=${state.sortByDateInCommonOrder}` +
			`&MyCommentsFirst=${state.userCommentsFirst}`)
			.then(response => response.json())
			.then(result => {
				setState({
					...state,
					comments: result.items,
					loading: false,
					countPages: calculatePagesCount(result.totalCount, state.onPage),
					totalCount: result.totalCount
				});
				setReload(false);
			});
	}, [
		state.currentPage,
		state.onPage,
		state.sortByDateInCommonOrder,
		state.userCommentsFirst,
		reload
	])

	const handleChangePage = (_, newPage) => {
		if (state.currentPage === newPage) {
			setState({ ...state, loading: true });
			setReload(true);
		}
		else {
			setState({ ...state, currentPage: newPage, loading: true });
		}
	}

	const handleCreateComment = () => {
		const total = state.totalCount + 1;
		setState({ ...state, totalCount: total, countPages: calculatePagesCount(total, state.onPage), loading: true });
		setReload(true);
	}

	const handleUpdateComment = (updComment) => {
		const arr = [...state.comments];
		let found = arr.find(c => c.id === updComment.id);
		if (found) {
			found.text = updComment.text;
		}
		setState({ ...state, comments: [...arr] });
	}

	const handleDeleteComment = (_) => {
		const total = state.totalCount - 1;
		const pages = calculatePagesCount(total, state.onPage);
		const currentPage = pages < state.countPages && state.currentPage > pages ? pages : state.currentPage;
		handleChangePage({}, currentPage);
	}

	const handleSortByDateOrderChanged = (_) => {
		setState({
			...state,
			currentPage: 1,
			sortByDateInCommonOrder: !state.sortByDateInCommonOrder,
			loading: true
		});
	}

	const handleUserCommentsFirstChanged = (_) => {
		setState({
			...state,
			currentPage: 1,
			userCommentsFirst: !state.userCommentsFirst,
			loading: true
		});
	}

	const handleChangeOnPageCount = (event) => {
		const newCount = event.target.value;
		setState({ ...state, currentPage: 1, onPage: newCount, loading: true });
	}

	return (
		<>
			<Box className={classes.header}>
				<Typography variant="h5">Мнения пользователей о фильме</Typography>
				{
					state.comments.length !== 0
						? <Box className={classes.listNavigation}>
							<Pagination
								page={Number(state.currentPage)}
								count={state.countPages}
								variant="outlined"
								color="primary"
								onChange={handleChangePage}
							/>
							<Box className={classes.leftMargin}>
								<OnPageCountSwitcher count={state.onPage} onChange={handleChangeOnPageCount} />
							</Box>
							<Button variant="primary" onClick={handleSortByDateOrderChanged}>
								{
									state.sortByDateInCommonOrder
										? <ArrowDownwardIcon />
										: <ArrowUpwardIcon />
								}
							</Button>
							<FormControlLabel
								control=
								{
									<Checkbox
										checked={state.userCommentsFirst}
										onChange={handleUserCommentsFirstChanged}
										color="primary"
										disabled={user.userName === null} />
								}
								label="Сначала мои" />
						</Box>
						: null
				}
			</Box>
			{
				state.loading
					? <Typography>Загрузка...</Typography>
					: <>
						{
							state.comments.length !== 0
								? <>
									<List className={classes.list}>
										{
											state.comments.map(comment => {
												return (
													<ListItem key={comment.Id}>
														<CommentTile
															comment={comment}
															onUpdate={handleUpdateComment}
															onDelete={handleDeleteComment}
														/>
													</ListItem>
												)
											})
										}
									</List>
								</>
								: <Typography variant='subtitle1'>Пока нет комментариев</Typography>
						}
					</>
			}
			{
				user.userName
					? <CommentArea filmId={props.filmId} onAdd={handleCreateComment} />
					: <Typography variant='subtitle1'>
						<Link to="/sign-in">Войдите</Link>, чтобы оставить свой комментарий
					</Typography>
			}
		</>
	)
}

export default CommentsList
