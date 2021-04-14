import React, { useState } from 'react'
import { CommentsList } from './CommentsList'
import { useParams } from "react-router-dom";
import FilmCard from './FilmCard'
import { NavMenu } from './NavMenu'
import { Typography } from '@material-ui/core';

function FilmPage() {
	let { slug } = useParams();
	const [film, setFilm] = useState(null);

	const fetchFilm = () => {
		fetch(`api/films/${slug}`)
			.then(response => response.json())
			.then(result => setFilm(result));
	}

	fetchFilm();
	return (
		<div>
			<NavMenu />
			{
				console.log(film)
			}
			{
				film !== null
					? <FilmCard film={film} />
					: <Typography>Загрузка...</Typography>
			}
			<CommentsList filmId={slug} />
		</div>
	)
}

export default FilmPage
