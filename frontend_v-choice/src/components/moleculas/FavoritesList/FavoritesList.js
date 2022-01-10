import React, { Component } from 'react'
import { Link } from 'react-router-dom'
import { Button, List, ListItem, Typography } from '@material-ui/core'
import ClearIcon from '@material-ui/icons/Clear'

import styles from './FavoritesList.module.css'

export class FavoritesList extends Component {
	constructor(props) {
		super(props);
		this.state = {
			favoriteFilms: [],
			loading: true,
			pageNumber: 1,
			onPage: 5,
			totalFilms: 0
		};
	}

	componentDidMount() {
		this.fetchFavorites();
	}

	async fetchFavorites() {
		fetch(`https://localhost:5001/api/favorite?pagenumber=${this.state.pageNumber}&onpagecount=${this.state.onPage}`)
			.then(response => response.json())
			.then(result => this.setState({
				favoriteFilms: result.items,
				loading: false,
				totalFilms: result.totalCount
			}));
	}

	removeItem = (film) => {
		this.setState({ favoriteFilms: this.state.favoriteFilms.filter(f => f.id !== film.id) });
	}

	handleRemoveItem = (film) => {
		fetch(`https://localhost:5001/api/favorite`, {
			method: 'DELETE',
			headers: {
				'Content-Type': 'application/json;charset=utf-8'
			},
			body: JSON.stringify(film)
		});
		this.removeItem(film);
	}

	render() {
		return (
			<div>
				{
					this.state.loading
						? <Typography>Загрузка...</Typography>
						: <List className={styles.list}>
							{
								this.state.favoriteFilms.length !== 0
									? this.state.favoriteFilms.map(film => {
										return (
											<ListItem key={film.id} className={styles.item}>
												<Link to={`/film/${film.id}`}>{film.title}</Link>
												<Button
													variant="primary"
													onClick={() => {
														return this.handleRemoveItem(film)
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
				}
			</div>
		)
	}
}

export default FavoritesList