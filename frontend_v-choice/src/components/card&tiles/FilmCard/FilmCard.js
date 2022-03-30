import { useState } from 'react'
import { createStyles, makeStyles, Box, Card, Typography } from '@material-ui/core'

import UpdateFilmDialog from './../../crud/UpdateFilmDialog/UpdateFilmDialog'
import DeleteFilm from './../../crud/DeleteFilm/DeleteFilm'
import { Link } from 'react-router-dom'

const useStyles = makeStyles((theme) => createStyles({
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
}));

function FilmCard(props) {
	const classes = useStyles();
	const [film, setFilm] = useState({ ...props.film });

	const handleOnUpdateFilm = (film) => {
		setFilm({ ...film });
		props.onUpdate(film);
	}

	return (
		<Card className={classes.filmCard}>
			<Box className={classes.cardItem && classes.cardVerticalSection}>
				<Typography variant='h4' className={classes.filmTitle}>
					{film.title}
				</Typography>
				<Typography className={classes.filmYear}>
					{film.year}
				</Typography>
			</Box>
			<Typography className={classes.cardItem && classes.filmDescription}>
				{film.description}
			</Typography>
			<Box className={classes.cardItem && classes.cardVerticalSection}>
				{
					(film.genres !== undefined) && (film.genres.lenght !== 0)
						? <Box className={classes.genresBox}>
							{
								film.genres.map(genre => {
									return (
										<Typography key={genre.Id} className={classes.filmGenre}>
											{genre.value}
										</Typography>
									)
								})
							}
						</Box>
						: <Typography className={classes.filmGenre}>
							Жанры не выбраны
						</Typography>
				}
			</Box>
			<Box className={classes.controlsContainer}>
				<Link to={`/film/${film.id}`}>Подробнее</Link >
				<Box className={classes.btns}>
					<UpdateFilmDialog film={film} onUpdate={handleOnUpdateFilm} />
					<DeleteFilm
						film={film}
						btnStyle={classes.btnDelete}
						onDelete={props.onDelete}
					/>
				</Box>
			</Box>
		</Card >
	)
}

export default FilmCard
