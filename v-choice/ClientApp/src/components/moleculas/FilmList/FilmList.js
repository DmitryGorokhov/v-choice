import React, { Component } from 'react'
import { Box, List, ListItem, Typography } from '@material-ui/core'
import { withStyles } from "@material-ui/core/styles"

import FilmCard from '../../card&tiles/FilmCard/FilmCard'
import AddFilmDialog from '../../crud/AddFilmDialog/AddFilmDialog'

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

	render() {
		return (
			<Box>
				<Box className={this.props.classes.tools}>
					<Typography variant="subtitle1">
						Инструменты
					</Typography>
					<AddFilmDialog genres={this.state.genres} />
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
												<FilmCard film={film} />
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
