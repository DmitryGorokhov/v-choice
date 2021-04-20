import React, { Component } from 'react'
import { Button, List, ListItem, Typography } from '@material-ui/core'
import { Link } from 'react-router-dom';

export class FavoritesList extends Component {
	constructor(props) {
		super(props);
		this.state = { favoriteFilms: [], loading: true };
	}

	componentDidMount() {
		this.fetchFavorites();
	}

	async fetchFavorites() {
		fetch(`api/user`)
			.then(response => response.json())
			.then(result => this.setState({ favoriteFilms: result, loading: false }));
	}

	removeItem = (film) => {
		console.log(`Убрать из списка элемент ${film.Title}`);
		// let ind = this.state.favoriteFilms.findIndex(c => c.Id === commentId);
		// this.state.favoriteFilms.splice(ind, 1);
		// this.setState({ favoriteFilms: this.state.favoriteFilms });
	}

	handleRemoveItem = (film) => {
		// fetch(`/api/user`, {
		// 	method: 'DELETE',
		// 	headers: {
		// 		'Content-Type': 'application/json;charset=utf-8'
		// 	},
		// 	body: JSON.stringify(film)
		// });
		this.removeItem(film);
	}

	render() {
		return (
			<div>
				{
					this.state.loading
						? <Typography>Загрузка...</Typography>
						:
						<List>
							{
								this.state.favoriteFilms.length !== 0
									? this.state.favoriteFilms.map(film => {
										return (
											<ListItem key={film.Id}>
												<Link to={`/film/${film.Id}`}>{film.Title}</Link>
												<Button variant="primary" onClick={this.handleRemoveItem(film)}>
													Убрать
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