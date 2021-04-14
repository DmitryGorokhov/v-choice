import React, { useState } from 'react'
import { useParams } from "react-router-dom"
import { Typography } from '@material-ui/core'
import FilmCard from '../../card&tiles/FilmCard/FilmCard'
import { NavMenu } from '../../atoms/NavMenu/NavMenu'
import { CommentsList } from '../../moleculas/CommentsList/CommentsList';

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
