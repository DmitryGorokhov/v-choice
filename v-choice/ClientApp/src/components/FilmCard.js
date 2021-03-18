import React, { Component } from 'react'
import { Box, Button, Card, Typography } from '@material-ui/core'
import { withStyles } from "@material-ui/core/styles";

const styles = (theme) => ({
	btns: {
		display: 'inline-block',
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
	filmCard: {
		margin: theme.spacing(0, 0, 2),
		padding: theme.spacing(3)
	},
	filmDescription: {
		fontSize: '18px',
		lineHeight: "150%"
	},
	filmGenre: {
		display: 'inline-block',
		fontSize: '14px',
		color: '#993333',
		marginRight: theme.spacing(1)
	},
	filmTitle: {
		textIndent: theme.spacing(5)
	},
	filmYear: {
		fontWeight: 'bold',
		fontSize: '16px'
	},
	genresBox: {
		width: '50%',
		padding: theme.spacing(0.5)
	}
});

class FilmCard extends Component {
	render() {
		return (
			<Card className={this.props.classes.filmCard}>
				<Box className={this.props.classes.cardItem && this.props.classes.cardVerticalSection}>
					<Typography variant='h4' className={this.props.classes.filmTitle}>
						{this.props.film.Title}
					</Typography>
					<Typography className={this.props.classes.filmYear}>
						{this.props.film.Year}
					</Typography>
				</Box>
				<Typography className={this.props.classes.cardItem && this.props.classes.filmDescription}>
					{this.props.film.Description}
				</Typography>
				<Box className={this.props.classes.cardItem && this.props.classes.cardVerticalSection}>
					{
						this.props.film.Genres.lenght !== 0
							? <Box className={this.props.classes.genresBox}>
								{
									this.props.film.Genres.map(genre => {
										return (
											<Typography className={this.props.classes.filmGenre}>
												{genre.Value}
											</Typography>
										)
									})
								}
							</Box>
							: <Typography className={this.props.classes.filmGenre}>
								Жанры не выбраны
							</Typography>
					}
					<Box className={this.props.classes.btns}>
						<Button variant="outlined" color="primary">
							Изменить
						</Button>
						<Button variant="outlined" color="secondary"
							className={this.props.classes.btnDelete}>
							Удалить
						</Button>
					</Box>
				</Box>
			</Card >
		)
	}
}

export default withStyles(styles)(FilmCard)
