import React, { Component } from 'react'
import { Box, List, ListItem, Typography } from '@material-ui/core'
import { withStyles } from "@material-ui/core/styles";
import FilmCard from './FilmCard'

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
		if (this.props.type === 'all') {
			let fetchURL = 'api/films';
			this.fetchFilmsData(fetchURL);
		}
	}

	async fetchFilmsData(fetchURL) {
		const response = await fetch(fetchURL);
		const data = await response.json();
		this.setState({ films: data, loading: false });
	}

	render() {
		return (
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
		)
	}
}

export default withStyles(styles)(FilmList)
