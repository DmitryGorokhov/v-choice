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

});

class FilmList extends Component {
	constructor(props) {
		super(props);
		this.state = { films: [], loading: true };
	}

	componentDidMount() {
		let fetchURL = 'api/films';
		this.fetchFilmsData(fetchURL);
	}

	async fetchFilmsData(fetchURL) {
		const response = await fetch(fetchURL);
		const data = await response.json();
		this.setState({ films: data, loading: false });
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

	render() {
		return (
			<Box>
				<AddFilmDialog foo={this.addCreatedFilm} />
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
												<FilmCard film={film} onDelete={this.removeFilm} />
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
