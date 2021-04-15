import React, { useState } from 'react'
import { useParams } from "react-router-dom"
import { Typography } from '@material-ui/core'

import FilmCard from '../../card&tiles/FilmCard/FilmCard'
import { NavMenu } from '../../atoms/NavMenu/NavMenu'
import { CommentsList } from '../../moleculas/CommentsList/CommentsList'

function FilmPage() {
	let { slug } = useParams();
	const [film, setFilm] = useState(null);
	const [userEmail, setUserEmail] = useState(null);

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
	}

	loadPage();
	return (
		<div>
			<NavMenu />
			{
				film !== null
					? <FilmCard film={film} />
					: <Typography>Загрузка...</Typography>
			}
			<CommentsList filmId={slug} userEmail={userEmail} />
		</div>
	)
}

export default FilmPage
