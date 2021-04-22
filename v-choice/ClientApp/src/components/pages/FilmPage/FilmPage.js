import React, { useEffect, useState } from 'react'
import { useParams } from "react-router-dom"
import { createStyles, makeStyles, Box, Button, Typography } from '@material-ui/core'
import { Link } from 'react-router-dom'
import BookmarkIcon from '@material-ui/icons/Bookmark'
import BookmarkBorderIcon from '@material-ui/icons/BookmarkBorder'

import FilmCard from '../../card&tiles/FilmCard/FilmCard'
import { NavMenu } from '../../atoms/NavMenu/NavMenu'
import { CommentsList } from '../../moleculas/CommentsList/CommentsList'
import styles from './FilmPage.module.css'

const useStyles = makeStyles((theme) => createStyles({
	marginItem: {
		margin: theme.spacing(2),
	},
	container: {
		margin: theme.spacing(2),
		padding: theme.spacing(1),
	}
}));

function FilmPage() {
	let { slug } = useParams();
	const [film, setFilm] = useState(null);
	const [userEmail, setUserEmail] = useState(null);
	const [disableAddButton, setDisableAddButton] = useState(true);

	useEffect(() => {
		fetch(`api/films/${slug}`)
			.then(response => response.json())
			.then(result => setFilm(result));

		fetch("api/account/isAuthenticated", {
			method: 'POST',
			headers: {
				'Content-Type': 'application/json'
			},
		})
			.then(response => response.json())
			.then(result => {
				result.message === "guest"
					? setUserEmail(null)
					: setUserEmail(result.message)
			});
		fetch(`api/user/${slug}`)
			.then(response => response.json())
			.then(result => setDisableAddButton(result));
	}, [])

	const handleAddFavorite = () => {
		fetch(`/api/user`, {
			method: 'PUT',
			headers: {
				'Content-Type': 'application/json;charset=utf-8'
			},
			body: JSON.stringify(film)
		});
		setDisableAddButton(true);
	}

	return (
		<div>
			<NavMenu />
			<Box className={styles.container}>

				{
					film !== null
						? <Box className={styles.marginItem}>
							<FilmCard film={film} />
							<Box className={styles.favoriteSection}>
								<Box className={styles.favoriteTextItem}>
									<Typography variant="h5">
										Заинтересовал фильм и не хотите его потерять? Добавьте в избранное.<br />Список избранных фильмов доступен в профиле.<br />
										<Link className={styles.text} to="/user">Мой профиль</Link>
									</Typography>
								</Box>

								<Button
									className={styles.favoriteButton}
									disabled={disableAddButton}
									onClick={handleAddFavorite}
								>
									{
										disableAddButton
											?
											<div>
												<BookmarkIcon />
										Уже добавлен
									</div>
											: <div>
												<BookmarkBorderIcon />
										Добавить в Избранное
										</div>
									}
								</Button>
							</Box>
						</Box>
						: <Typography>Загрузка...</Typography>
				}
				<Typography variant="h4" className={styles.marginItem}>
					Мнения пользователей о фильме
				</Typography>
				<CommentsList className={styles.marginItem} filmId={slug} userEmail={userEmail} />
			</Box>
		</div>
	)
}

export default FilmPage
