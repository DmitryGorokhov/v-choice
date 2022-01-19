import React, { useEffect, useState } from 'react'
import { useParams } from "react-router-dom"
import { createStyles, makeStyles, Box, Container, Typography } from '@material-ui/core'

import FilmList from '../../moleculas/FilmList/FilmList'
import { NavMenu } from '../../atoms/NavMenu/NavMenu'

const useStyles = makeStyles((theme) => createStyles({
	headerContainer: {
		display: 'flex',
		margin: theme.spacing(2),
		padding: theme.spacing(1),
		justifyContent: 'space-between'
	},
}));

function Films() {
	const [state, setState] = useState({
		genres: [],
	});

	useEffect(() => {
		fetch('https://localhost:5001/api/genre')
			.then(response => response.json())
			.then(result => setState({ ...state, genres: result }));
	}, [])

	let { page, count, genre, type, order, onlyc, norate } = useParams();

	// Check url params
	onlyc = (onlyc === undefined || Number(onlyc) === NaN) ? 0 : Number(onlyc);
	norate = (norate === undefined || Number(norate) === NaN) ? 0 : Number(norate);

	const classes = useStyles();

	const params = {
		pageNumber: (page === undefined || Number(page) === NaN) ? 1 : Number(page),
		onPage: (count === undefined || Number(count) === NaN) ? 3 : Number(count),
		genre: (genre === undefined || Number(genre) === NaN) ? -1 : Number(genre),
		sortType: (type === undefined || Number(type) === NaN) ? 0 : Number(type),
		order: !(Number(order) === 0),
		onlyComments: Number(order) === 1,
		noUserRate: Number(order) === 1,
	}

	return (
		<div>
			<NavMenu />
			<Container>
				<Box className={classes.headerContainer}>
					<Typography variant="h2">
						Фильмы
					</Typography>
				</Box>
				<FilmList {...params} genres={state.genres} />
			</Container>
		</div>
	)
}

export default Films
