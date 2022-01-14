import React, { useEffect, useState } from 'react'
import { Link } from 'react-router-dom'
import { Button, List, ListItem, Typography } from '@material-ui/core'
import ClearIcon from '@material-ui/icons/Clear'
import Pagination from '@material-ui/lab/Pagination'

import styles from './FavoritesList.module.css'

function FavoritesList() {
	const [state, setState] = useState({
		onPage: 5,
		favorites: [],
		loading: true,
		totalCount: 0,
		currentPage: 1
	});

	useEffect(() => {
		fetch(`https://localhost:5001/api/favorite?pagenumber=${state.currentPage}&onpagecount=${state.onPage}`)
			.then(response => response.json())
			.then(result => setState({
				...state,
				favorites: result.items,
				loading: false,
				totalCount: result.totalCount
			}));
	}, [state.currentPage])

	const removeItem = (film) => {
		setState({ ...state, favorites: state.favorites.filter(f => f.id !== film.id) });
	}

	const handleRemoveItem = (film) => {
		fetch(`https://localhost:5001/api/favorite`, {
			method: 'DELETE',
			headers: {
				'Content-Type': 'application/json;charset=utf-8'
			},
			body: JSON.stringify(film)
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

	return (
		<div>
			{
				state.loading
					? <Typography>Загрузка...</Typography>
					:
					<>
						<List className={styles.list}>
							{
								state.favorites.length !== 0
									? state.favorites.map(film => {
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
									: <Typography variant='h5'>Список избранных фильмов пуст</Typography>
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
		</div>
	)
}

export default FavoritesList