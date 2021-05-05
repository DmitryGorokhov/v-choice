import React, { Component } from 'react'
import { Box, List, ListItem, Typography } from '@material-ui/core'
import { withStyles } from "@material-ui/core/styles"

import FilmCard from '../../card&tiles/FilmCard/FilmCard'
import AddFilmDialog from '../../crud/AddFilmDialog/AddFilmDialog'
import FilmsFilter from '../../atoms/FilmsFilter/FilmsFilter'

const styles = (theme) => ({
	filmListItem: {
		display: 'block'
	},
	loading: {
		margin: theme.spacing(1),
		fontSize: '20px'
	},
	tools: {
		display: 'flex',
		justifyContent: 'space-between',
		alignItems: 'center',
		margin: theme.spacing(0, 2),
	}
});

class FilmList extends Component {
	constructor(props) {
		super(props);
		this.state = { genres: [], films: [], loading: true };
		this.allFilms = [];
	}

	componentDidMount() {
		this.fetchGenresData();
		this.fetchFilmsData();
	}

	async fetchFilmsData() {
		fetch('api/films')
			.then(response => response.json())
			.then(result => {
				this.allFilms = result;
				this.setState({ films: result });
			});
	}

	async fetchGenresData() {
		fetch('api/genres')
			.then(response => response.json())
			.then(result => this.setState({ genres: result, loading: false }));
	}

	updateFilmList = (filmList) => {
		this.setState({ films: filmList })
	}

	showAll = () => {
		this.setState({ films: this.allFilms });
	}

	updateFilm = (film) => {
		let ind = this.state.films.findIndex(c => c.Id === film.Id);
		let arr = this.state.films;
		arr[ind] = film;
		ind = this.state.allFilms.findIndex(c => c.Id === film.Id);
		let all = this.state.allFilms;
		all[ind] = film;
		this.setState({ films: arr, allFilms: all });
	}

	deleteFilm = (film) => {
		this.setState({
			films: this.state.films.filter(f => f.Id !== film.Id),
			allFilms: this.state.allFilms.filter(f => f.Id !== film.Id)
		});
	}

	render() {
		return (
			<div>
				{
					this.state.loading
						? <Typography className={this.props.classes.loading}>
							Загрузка...
							</Typography >
						: <Box>
							<Box className={this.props.classes.tools}>
								<Typography variant="subtitle1">
									Инструменты
								</Typography>
								<FilmsFilter
									onFilter={this.updateFilmList}
									genres={this.state.genres}
									loadAll={this.showAll}
								/>
								<AddFilmDialog genres={this.state.genres} />
							</Box>
							<Box>
								<List>
									{
										this.state.films.length !== 0
											? this.state.films.map(film => {
												return (
													<ListItem
														className={this.props.classes.filmListItem}
														key={film.Id}
													>
														<FilmCard
															film={film}
															onUpdate={this.updateFilm}
															onDelete={this.deleteFilm}
														/>
													</ListItem>
												)
											})
											: <Typography variant="h5">
												Нет фильмов с выбранным жанром
												</Typography>
									}
								</List>
							</Box>
						</Box>
				}
			</div>
		)
	}
}

export default withStyles(styles)(FilmList)
