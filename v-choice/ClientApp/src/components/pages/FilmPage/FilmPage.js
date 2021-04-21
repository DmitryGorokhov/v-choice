import React, { useState } from 'react'
import { useParams } from "react-router-dom"
import { Button, Typography } from '@material-ui/core'
import { Link } from 'react-router-dom';

import FilmCard from '../../card&tiles/FilmCard/FilmCard'
import { NavMenu } from '../../atoms/NavMenu/NavMenu'
import { CommentsList } from '../../moleculas/CommentsList/CommentsList'

function FilmPage() {
	let { slug } = useParams();
	const [film, setFilm] = useState(null);
	const [userEmail, setUserEmail] = useState(null);
	const [disableAddButton, setDisableAddButton] = useState(true);

	const loadPage = () => {
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
	}

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

	loadPage();
	return (
		<div>
			<NavMenu />
			{
				film !== null
					? <div>
						<FilmCard film={film} />
						<div>
							<Typography>
								Заинтересовал фильм и не хотите его потерять? Добавьте в избранное. Список избранных фильмов доступен в профиле.
							</Typography>
							<Link to="/user">Мой профиль</Link>
						</div>
						<Button disabled={disableAddButton}
							onClick={handleAddFavorite}>
							Добавить в избранное</Button>
					</div>
					: <Typography>Загрузка...</Typography>
			}

			<CommentsList filmId={slug} userEmail={userEmail} />
		</div>
	)
}

export default FilmPage
