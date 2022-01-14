import React, { useEffect, useState } from 'react'
import { useParams } from "react-router-dom"
import { createStyles, makeStyles, Box, Button, Typography } from '@material-ui/core'
import { Link } from 'react-router-dom'
import BookmarkIcon from '@material-ui/icons/Bookmark'
import BookmarkBorderIcon from '@material-ui/icons/BookmarkBorder'

import FilmCard from '../../card&tiles/FilmCard/FilmCard'
import { NavMenu } from '../../atoms/NavMenu/NavMenu'
import CommentsList from '../../moleculas/CommentsList/CommentsList'
import styles from './FilmPage.module.css'

function FilmPage() {
	let { slug } = useParams();
	const [film, setFilm] = useState(null);
	const [userEmail, setUserEmail] = useState(null);
	const [disableAddButton, setDisableAddButton] = useState(true);

	useEffect(() => {
		fetch(`https://localhost:5001/api/film/${slug}`)
			.then(response => response.json())
			.then(result => setFilm(result));

		fetch("https://localhost:5001/api/account/isAuthenticated", {
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
		fetch(`https://localhost:5001/api/favorite/${slug}`)
			.then(response => response.json())
			.then(result => setDisableAddButton(result));
	}, [])

	const handleAddFavorite = () => {
		fetch(`https://localhost:5001/api/favorite`, {
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
				<Box className={styles.marginItem}>
					{
						film !== null
							? <FilmCard film={film} />
							: <Typography>Загрузка...</Typography>
					}
					{
						userEmail !== null
							?
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
							: <Typography></Typography>

					}
				</Box>
				<Typography variant="h4" className={styles.marginItem}>
					Мнения пользователей о фильме
				</Typography>
				<CommentsList className={styles.marginItem} filmId={slug} userEmail={userEmail} />
			</Box>
		</div>
	)
}

export default FilmPage
