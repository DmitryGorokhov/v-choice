import React, { useContext, useEffect, useState } from 'react'
import { useParams } from "react-router-dom"
import { createStyles, makeStyles, Box, Button, Grid, Typography, } from '@material-ui/core'
import { Link } from 'react-router-dom'
import BookmarkIcon from '@material-ui/icons/Bookmark'
import BookmarkBorderIcon from '@material-ui/icons/BookmarkBorder'

import FilmCardWithoutControls from '../../card&tiles/FilmCardWithoutControls/FilmCardWithoutControls'
import CommentsList from '../../moleculas/CommentsList/CommentsList'
import RateArea from '../../atoms/RateArea/RateArea'
import UserContext from '../../../context'

const useStyles = makeStyles((theme) => createStyles({
	container: {
		margin: theme.spacing(0, 2),
	},
	favorite: {
		margin: theme.spacing(4, 0),
	},
	favText: {
		fontSize: '18px',
		lineHeight: "150%",
	},
	favBtnContainer: {
		display: 'flex',
		justifyContent: 'right',
	},
}));

function FilmPage() {
	const classes = useStyles();
	let { slug } = useParams();
	const { user, _ } = useContext(UserContext);
	const [film, setFilm] = useState(null);
	const [disableAddButton, setDisableAddButton] = useState(true);

	useEffect(() => {
		fetch(`https://localhost:5001/api/film/${slug}`)
			.then(response => response.json())
			.then(result => setFilm(result))
			.catch(_ => _);
		fetch(`https://localhost:5001/api/favorite/${slug}`)
			.then(response => response.json())
			.then(result => setDisableAddButton(result))
			.catch(_ => setDisableAddButton(true));
	}, [])

	const handleAddFavorite = () => {
		fetch(`https://localhost:5001/api/favorite/${film.id}`, {
			method: 'POST',
		});
		setDisableAddButton(true);
	}

	const handleRateChanged = (value, count) => {
		// Only one func for create, update or delete user rate,
		// so value can be newRate, newRate - oldRate, - oldRate.
		// Just add it to TotalRate.
		const newTotal = film.totalRate + value;
		// The same about count.
		const newCount = film.countRate + count;

		setFilm({
			...film,
			countRate: newCount,
			totalRate: newTotal,
			averageRate: newTotal / newCount
		});
	}

	return (
		<>
			<Box className={classes.container}>
				{
					film !== null
						?
						<>
							<FilmCardWithoutControls film={film} />
							<RateArea
								filmId={film.id}
								filmRate={film.averageRate}
								onAction={handleRateChanged} />
						</>
						: <Typography>Загрузка...</Typography>
				}
				{
					user.userName
						? <Grid container className={classes.favorite}>
							<Grid item xs={9}>
								<Typography className={classes.favText}>
									Заинтересовал фильм? Не хотите его потерять? - Добавьте в избранное<br />
									Список избранных фильмов доступен в <Link to="/user">профиле</Link>
								</Typography>
							</Grid>
							<Grid item xs={3}>
								<Box className={classes.favBtnContainer}>
									<Button disabled={disableAddButton} onClick={handleAddFavorite}>
										{
											disableAddButton
												? <> <BookmarkIcon /> Уже добавлен </>
												: <> <BookmarkBorderIcon /> Добавить в Избранное </>
										}
									</Button>
								</Box>
							</Grid>
						</Grid>
						: null
				}
				<CommentsList filmId={slug} />
			</Box>
		</>
	)
}

export default FilmPage
