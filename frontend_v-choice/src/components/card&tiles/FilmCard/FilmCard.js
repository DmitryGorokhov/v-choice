import { useState, useEffect } from 'react'
import { createStyles, makeStyles, Box, Card, Grid, Typography } from '@material-ui/core'

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
		marginTop: theme.spacing(2),
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
	},
	image: {
		width: 180,
		height: 220,
		objectFit: 'cover',
	}
}));

function FilmCard(props) {
	const classes = useStyles();
	const [film, setFilm] = useState({ ...props.film });
	const baseURL = 'https://localhost:5001/';
	const [picture, setPicture] = useState(`${baseURL}${film.posterPath}`);

	const handleOnUpdateFilm = (film) => {
		setFilm({ ...film });
		setPicture(`${baseURL}${film.posterPath}`);
		props.onUpdate(film);
	}

	useEffect(() => {
		let isMounted = true;
		const img = new Image();
		img.src = `${baseURL}${film.posterPath}`;

		if (isMounted) {
			img.onerror = () => setPicture(`${baseURL}img/empty.jpg`);
		}

		return () => {
			isMounted = false;
		};
	}, [picture]);

	return (
		<Card className={classes.filmCard}>
			<Grid container spacing={2}>
				<Grid item xs={10}>
					<Box className={classes.cardItem && classes.cardVerticalSection}>
						<Typography variant='h4' className={classes.filmTitle}>{film.title}</Typography>
						<Typography className={classes.filmYear}>{film.year}</Typography>
					</Box>
					<Typography className={classes.cardItem && classes.filmDescription}>
						{film.description}
					</Typography>
					<Box className={classes.cardItem && classes.cardVerticalSection}>
						<Box className={classes.genresBox}>
							{
								film.genres !== undefined && film.genres.length !== 0
									? film.genres.map(genre => {
										return (
											<Typography key={genre.Id} className={classes.filmGenre}>
												{genre.value}
											</Typography>
										)
									})
									: <Typography className={classes.filmGenre}>Жанры не выбраны</Typography>
							}
						</Box>
					</Box>
				</Grid>
				<Grid item xs={2}>
					<img className={classes.image} src={picture} alt={film.title} />
				</Grid>
			</Grid>
			<Box className={classes.controlsContainer}>
				<Link to={`/film/${film.id}`}>Подробнее</Link >
				{
					props.shouldShowControls
						? <Box className={classes.btns}>
							<UpdateFilmDialog film={film} onUpdate={handleOnUpdateFilm} genres={props.genres} />
							<DeleteFilm film={film} btnStyle={classes.btnDelete} onDelete={props.onDelete} />
						</Box>
						: null
				}
			</Box>
		</Card >
	)
}

export default FilmCard
