import { useContext, useState, useEffect } from 'react'
import { createStyles, makeStyles, Avatar, Box, Card, CardActionArea, CardContent, Grid, Typography } from '@material-ui/core'
import { AvatarGroup } from "@mui/material"
import { useHistory } from 'react-router-dom'

import UpdateFilmDialog from './../../crud/UpdateFilmDialog/UpdateFilmDialog'
import DeleteFilm from './../../crud/DeleteFilm/DeleteFilm'
import UserContext from '../../../context'

const useStyles = makeStyles((theme) => createStyles({
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
		margin: theme.spacing(2, 3),
		display: 'flex',
		justifyContent: 'right',
		alignItems: 'center'
	},
	card: {
		margin: theme.spacing(0, 0, 2),
	},
	cardContent: {
		padding: theme.spacing(3),
	},
	filmDescription: {
		fontSize: '16px',
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
	},
	avatarContainer: {
		display: 'flex',
		alignItems: 'center',
	},
	personText: {
		marginRight: theme.spacing(1),
	},
}));

function FilmCard(props) {
	const classes = useStyles();
	const history = useHistory();
	const { user, _ } = useContext(UserContext);
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

	const handleOpenFilmPage = (_) => history.replace({ pathname: `/film/${film.id}` });

	return (
		<Card className={classes.card}>
			<CardActionArea onClick={handleOpenFilmPage}>
				<CardContent className={classes.cardContent}>
					<Grid container spacing={2}>
						<Grid item xs={10}>
							<Box className={classes.cardItem && classes.cardVerticalSection}>
								<Typography variant='h5' className={classes.filmTitle}>{film.title}</Typography>
								<Typography className={classes.filmYear}>{film.year}</Typography>
							</Box>
							<Typography className={classes.cardItem && classes.filmDescription}>
								{film.description}
							</Typography>
							<Grid container spacing={2}>
								<Grid item xs={6}>
									<Box className={classes.avatarContainer}>
										<Typography className={classes.personText}>Режиссёры: </Typography>
										{
											film.directors.length !== 0
												? <AvatarGroup max={4}>
													{
														film.directors.map(p => <Avatar alt={p.fullName} src={`${baseURL}${p.photoPath}`} />)
													}
												</AvatarGroup>
												: <Typography> не выбраны</Typography>
										}
									</Box>
								</Grid>
								<Grid item xs={6}>
									<Box className={classes.avatarContainer}>
										<Typography className={classes.personText}>Актёры: </Typography>
										{
											film.cast.length !== 0
												? <AvatarGroup max={4}>
													{
														film.cast.map(p => <Avatar alt={p.fullName} src={`${baseURL}${p.photoPath}`} />)
													}
												</AvatarGroup>
												: <Typography> не выбраны</Typography>
										}
									</Box>
								</Grid>
							</Grid>
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
									{
										film.studio !== null
											? <Typography variant='subtitle2'>Студия: {film.studio.name}</Typography>
											: null
									}
								</Box>
							</Box>
						</Grid>
						<Grid item xs={2}>
							<img className={classes.image} src={picture} alt={film.title} />
						</Grid>
					</Grid>
				</CardContent>
			</CardActionArea>
			{
				user.isAdmin
					?
					<Box className={classes.controlsContainer}>
						<UpdateFilmDialog
							film={film}
							onUpdate={handleOnUpdateFilm}
							genres={props.genres}
							studios={props.studios}
							persons={props.persons}
						/>
						<DeleteFilm film={film} btnStyle={classes.btnDelete} onDelete={props.onDelete} />
					</Box>
					: null
			}
		</Card >
	)
}

export default FilmCard
