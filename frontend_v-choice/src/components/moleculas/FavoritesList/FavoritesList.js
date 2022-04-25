import React, { useEffect, useState } from 'react'
import { Link } from 'react-router-dom'
import { Button, List, ListItem, Typography } from '@material-ui/core'
import ClearIcon from '@material-ui/icons/Clear'
import Pagination from '@material-ui/lab/Pagination'
import ArrowDownwardIcon from '@material-ui/icons/ArrowDownward'
import ArrowUpwardIcon from '@material-ui/icons/ArrowUpward'

import styles from './FavoritesList.module.css'

function FavoritesList() {
	const [state, setState] = useState({
		onPage: 5,
		favorites: [],
		loading: true,
		totalCount: 0,
		currentPage: 1,
		sortByDateInCommonOrder: true,
	});

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
					totalCount: result.totalCount
				});
			})
	}, [state.currentPage, state.sortByDateInCommonOrder])

	const removeItem = (film) => {
		setState({ ...state, favorites: state.favorites.filter(f => f.id !== film.id) });
	}

	const handleRemoveItem = (film) => {
		fetch(`https://localhost:5001/api/Favorite/${film.id}`, {
			method: 'DELETE',
		});
		removeItem(film);
	}

	const calculatePagesCount = () => {
		let value = Math.floor(state.totalCount / state.onPage)
		return value * state.onPage === state.totalCount ? value : value + 1
	}

	const handleChangePage = (event, newPage) => {
		setState({ ...state, currentPage: newPage, loading: true });
	}

	const handleSortByDateOrderChanged = (event) => {
		setState({
			...state,
			currentPage: 1,
			sortByDateInCommonOrder: !state.sortByDateInCommonOrder,
			loading: true
		});
	}

	return (
		<>
			{
				state.loading
					? <Typography>Загрузка...</Typography>
					:
					<>
						{
							state.favorites.length !== 0
								? <>
									<List className={styles.list}>
										{
											state.favorites.map(film => {
												return (
													<ListItem key={film.id} className={styles.item}>
														<Link to={`/film/${film.id}`}>{film.title}</Link>
														<Button
															variant="primary"
															onClick={() => {
																return handleRemoveItem(film)
															}}
														>
															<ClearIcon />
														</Button>
													</ListItem>
												)
											})
										}
									</List>
									<Pagination
										page={Number(state.currentPage)}
										count={calculatePagesCount()}
										variant="outlined"
										color="primary"
										onChange={handleChangePage}
									/>
									<Button
										variant="primary"
										onClick={handleSortByDateOrderChanged}
									>
										{
											state.sortByDateInCommonOrder
												? <ArrowDownwardIcon />
												: <ArrowUpwardIcon />
										}
									</Button>
								</>
								: <Typography variant='h5'>Список избранных фильмов пуст</Typography>
						}
					</>
			}
		</>
	)
}

export default FavoritesList