import React, { Component } from 'react'
import { Box, List, ListItem, Typography } from '@material-ui/core'
import { withStyles } from "@material-ui/core/styles";
import FilmCard from './FilmCard'
import AddFilmDialog from './AddFilmDialog'

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
		alignItems: 'centre',
		margin: theme.spacing(0, 2),
	}
});

class FilmList extends Component {
	constructor(props) {
		super(props);
		this.state = { genres: [], films: [], loading: true };
	}

	componentDidMount() {
		this.fetchGenresData('api/genres');
		this.fetchFilmsData('api/films');
	}

	async fetchFilmsData(fetchURL) {
		fetch(fetchURL)
			.then(response => response.json())
			.then(result => this.setState({ films: result }));
	}

	async fetchGenresData(fetchURL) {
		fetch(fetchURL)
			.then(response => response.json())
			.then(result => this.setState({ genres: result, loading: false }));
	}

	addCreatedFilm = (film) => {
		this.state.films.push(film);
		this.setState({ films: this.state.films });
	}

	removeFilm = (film) => {
		let index = this.state.films.indexOf(film);
		if (index !== -1) this.state.films.splice(index, 1);
		this.setState({ films: this.state.films });
	}

	updateFilm = (film) => {
		let index = this.state.films.map(f => {
			if (f.Id === film.Id) {
				return (this.state.films.indexOf(f));
			}
		})
		this.state.films[index] = film;
		this.setState({ films: this.state.films });
	}

	render() {
		return (
			<Box>
				<Box className={this.props.classes.tools}>
					<Typography variant="subtitle1">
						Инструменты
					</Typography>
					<AddFilmDialog foo={this.addCreatedFilm} genres={this.state.genres} />
				</Box>
				<Box>
					{
						this.state.loading
							? <Typography className={this.props.classes.loading}>
								Загрузка...
							</Typography >
							: <List>
								{
									this.state.films.map(film => {
										return (
											<ListItem className={this.props.classes.filmListItem} key={film.Id}>
												<FilmCard film={film}
													onDelete={this.removeFilm}
													onUpdate={this.updateFilm}
													genres={this.state.genres}
												/>
											</ListItem>
										)
									})
								}
							</List>
					}
				</Box>
			</Box>
		)
	}
}

export default withStyles(styles)(FilmList)
