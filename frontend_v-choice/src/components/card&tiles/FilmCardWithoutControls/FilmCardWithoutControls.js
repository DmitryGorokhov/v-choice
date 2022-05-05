import { useState, useEffect } from 'react'
import { createStyles, makeStyles, Box, Card, Grid, Typography } from '@material-ui/core'

const useStyles = makeStyles((theme) => createStyles({
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
		padding: theme.spacing(2, 3)
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

function FilmCardWithoutControls(props) {
	const classes = useStyles();
	const film = props.film;
	const baseURL = 'https://localhost:5001/';
	const [picture, setPicture] = useState(`${baseURL}${film.posterPath}`);

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
		</Card >
	)
}

export default FilmCardWithoutControls
