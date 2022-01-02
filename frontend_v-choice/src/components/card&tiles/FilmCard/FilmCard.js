import React, { Component } from 'react'
import { Box, Card, Typography } from '@material-ui/core'
import { withStyles } from "@material-ui/core/styles"

import UpdateFilmDialog from './../../crud/UpdateFilmDialog/UpdateFilmDialog'
import DeleteFilm from './../../crud/DeleteFilm/DeleteFilm'
import { Link } from 'react-router-dom'

const styles = (theme) => ({
	btns: {
		display: 'flex',
	},
	btnDelete: {
		marginLeft: theme.spacing(2)
	},
	cardItem: {
		marginBottom: theme.spacing(3)
	},
	cardVerticalSection: {
		display: 'flex',
		justifyContent: "space-between",
		alignItems: 'center'
	},
	controlsContainer: {
		marginTop: theme.spacing(3),
		display: 'flex',
		justifyContent: 'space-between',
		alignItems: 'center'
	},
	filmCard: {
		margin: theme.spacing(0, 0, 2),
		padding: theme.spacing(3)
	},
	filmDescription: {
		fontSize: '18px',
		lineHeight: "150%",
		marginBottom: theme.spacing(2)
	},
	filmGenre: {
		display: 'inline-block',
		fontSize: '14px',
		color: '#993333',
		marginRight: theme.spacing(1)
	},
	filmTitle: {
		textIndent: theme.spacing(5),
		marginBottom: theme.spacing(2)
	},
	filmYear: {
		fontWeight: 'bold',
		fontSize: '16px'
	},
	genresBox: {
		width: '100%',
		padding: theme.spacing(0.5)
	}
});

class FilmCard extends Component {
	constructor(props) {
		super(props);
	}

	film = this.props.film;
	classes = this.props.classes;

	render() {
		return (
			<Card className={this.classes.filmCard}>
				<Box className={this.classes.cardItem && this.classes.cardVerticalSection}>
					<Typography variant='h4' className={this.classes.filmTitle}>
						{this.film.title}
					</Typography>
					<Typography className={this.classes.filmYear}>
						{this.film.year}
					</Typography>
				</Box>
				<Typography className={this.classes.cardItem && this.classes.filmDescription}>
					{this.film.description}
				</Typography>
				<Box className={this.classes.cardItem && this.classes.cardVerticalSection}>
					{
						(this.film.genres !== undefined) && (this.film.genres.lenght !== 0)
							? <Box className={this.classes.genresBox}>
								{
									this.film.genres.map(genre => {
										return (
											<Typography key={genre.Id} className={this.classes.filmGenre}>
												{genre.value}
											</Typography>
										)
									})
								}
							</Box>
							: <Typography className={this.classes.filmGenre}>
								Жанры не выбраны
							</Typography>
					}
				</Box>
				<Box className={this.classes.controlsContainer}>
					<Link to={`/film/${this.film.id}`}>Подробнее</Link >
					<Box className={this.classes.btns}>
						<UpdateFilmDialog film={this.film} />
						<DeleteFilm
							film={this.film}
							btnStyle={this.classes.btnDelete}
						/>
					</Box>
				</Box>
			</Card >
		)
	}
}

export default withStyles(styles)(FilmCard)
