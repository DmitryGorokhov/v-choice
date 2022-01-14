import React, { useEffect, useState } from 'react'
import { List, ListItem, Typography } from '@material-ui/core'
import Pagination from '@material-ui/lab/Pagination'

import CommentTile from '../../card&tiles/CommentTile/CommentTile'
import CommentArea from '../../atoms/CommentArea/CommentArea'
import styles from './CommentsList.module.css'


function CommentsList(props) {
	const filmId = props.filmId;
	const [state, setState] = useState({
		onPage: 3,
		comments: [],
		loading: true,
		totalCount: 0,
		currentPage: 1
	});

	useEffect(() => {
		fetch(`https://localhost:5001/api/Comment?PageNumber=${state.currentPage}&OnPageCount=${state.onPage}&FilmId=${filmId}`)
			.then(response => response.json())
			.then(result => setState({ ...state, comments: result.items, loading: false, totalCount: result.totalCount }));
	}, [state.currentPage])

	const updateComment = (updComment) => {
		let ind = state.comments.findIndex(c => c.id === updComment.id);
		state.comments[ind].text = updComment.text;
		setState({ ...state, comments: state.comments });
	}

	const deleteComment = (commentId) => {
		let ind = state.comments.findIndex(c => c.id === commentId);
		state.comments.splice(ind, 1);
		setState({ ...state, comments: state.comments });
	}

	const calculatePagesCount = () => {
		let value = Math.floor(state.totalCount / state.onPage)
		return value * state.onPage === state.totalCount ? value : value + 1
	}

	const handleChangePage = (event, newPage) => {
		setState({ ...state, currentPage: newPage, loading: true });
	}

	return (
		<div>
			{
				state.loading
					? <Typography>Загрузка...</Typography>
					:
					<>
						<List className={styles.list}>
							{
								state.comments.length !== 0
									? state.comments.map(comment => {
										return (
											<ListItem className={styles.listItem} key={comment.Id}>
												<CommentTile
													comment={comment}
													userEmail={props.userEmail}
													onUpdateMethod={updateComment}
													onDeleteMethod={deleteComment}
												/>
											</ListItem>
										)
									})
									: <Typography variant='h5'>Пока нет комментариев</Typography>
							}
						</List>
						<Pagination
							page={Number(state.currentPage)}
							count={calculatePagesCount()}
							variant="outlined"
							color="primary"
							onChange={handleChangePage}
						/>
					</>

			}

			{
				props.userEmail === null
					? <Typography variant='h6'>
						Авторизируйтесь, чтобы оставить свой комментарий
					</Typography>
					: <CommentArea filmId={filmId} typeMethod="create" />
			}
		</div>
	)
}

export default CommentsList
