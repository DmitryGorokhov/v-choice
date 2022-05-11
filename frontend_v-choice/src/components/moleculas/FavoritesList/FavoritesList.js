import React, { useEffect, useState } from 'react'
import { Link } from 'react-router-dom'
import { createStyles, makeStyles, Box, Button, IconButton, List, ListItem, Typography } from '@material-ui/core'
import ClearIcon from '@material-ui/icons/Clear'
import ArrowDownwardIcon from '@material-ui/icons/ArrowDownward'
import ArrowUpwardIcon from '@material-ui/icons/ArrowUpward'

import Pagination from '@material-ui/lab/Pagination'
import OnPageCountSwitcher from '../../atoms/OnPageCountSwitcher/OnPageCountSwitcher'

const useStyles = makeStyles((theme) => createStyles({
	container: {
		margin: theme.spacing(2),
		padding: theme.spacing(1),
	},
	header: {
		margin: theme.spacing(2, 0),
		display: 'flex',
		justifyContent: 'space-between',
	},
	item: {
		fontSize: '20px',
	},
	list: {
		width: '100%',
		maxHeight: '600px',
		overflowY: 'scroll',
	},
	leftMargin: {
		marginLeft: theme.spacing(2),
	},
	listNavigation: {
		display: 'flex',
		alignContent: 'center',
		alignItems: 'center',
	},
}));

function FavoritesList() {
	const classes = useStyles();
	const [state, setState] = useState({
		onPage: 5,
		favorites: [],
		loading: true,
		totalCount: 0,
		countPages: 0,
		currentPage: 1,
		sortByDateInCommonOrder: true,
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
			'https://localhost:5001/api/Favorite' +
			`?PageNumber=${state.currentPage}` +
			`&OnPageCount=${state.onPage}` +
			`&CommonOrder=${state.sortByDateInCommonOrder}`)
			.then(response => response.json())
			.then(result => {
				setState({
					...state,
					favorites: result.items,
					loading: false,
					countPages: calculatePagesCount(result.totalCount, state.onPage),
					totalCount: result.totalCount
				});
			})
	}, [state.currentPage, state.sortByDateInCommonOrder, state.onPage, reload])

	const handleRemoveItem = (film) => {
		fetch(`https://localhost:5001/api/Favorite/${film.id}`, {
			method: 'DELETE',
		});

		const total = state.totalCount - 1;
		const pages = calculatePagesCount(total, state.onPage);
		const currentPage = pages < state.countPages && state.currentPage > pages ? pages : state.currentPage;
		handleChangePage({}, currentPage);
	}

	const handleChangePage = (_, newPage) => {
		if (state.currentPage === newPage) {
			setState({ ...state, loading: true });
			setReload(true);
		}
		else {
			setState({ ...state, currentPage: newPage, loading: true });
		}
	}

	const handleChangeOnPageCount = (event) => {
		const newCount = event.target.value;
		setState({ ...state, currentPage: 1, onPage: newCount, loading: true });
	}

	const handleSortByDateOrderChanged = (_) => {
		setState({
			...state,
			currentPage: 1,
			sortByDateInCommonOrder: !state.sortByDateInCommonOrder,
			loading: true
		});
	}

	return (
		<>
			<Box className={classes.header}>
				<Typography variant='h5'>Избранные фильмы</Typography>
				{
					state.favorites.length !== 0
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
						</Box>
						: null
				}
			</Box>
			{
				state.loading
					? <Typography>Загрузка...</Typography>
					:
					<>
						{
							state.favorites.length !== 0
								? <>
									<List className={classes.list}>
										{
											state.favorites.map(film => {
												return (
													<ListItem key={film.id} className={classes.item}>
														<Link to={`/film/${film.id}`}>{film.title}</Link>
														<IconButton
															variant="primary"
															onClick={() => { return handleRemoveItem(film) }}
														>
															<ClearIcon />
														</IconButton>
													</ListItem>
												)
											})
										}
									</List>
								</>
								: <Typography variant='subtitle1'>Список избранных фильмов пуст</Typography>
						}
					</>
			}
		</>
	)
}

export default FavoritesList