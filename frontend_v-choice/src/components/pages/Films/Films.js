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

	let { page, count } = useParams();

	// Check url params
	if (page === undefined) {
		page = 1;
	}
	if (count === undefined) {
		count = 3;
	}

	const classes = useStyles();

	return (
		<div>
			<NavMenu />
			<Container>
				<Box className={classes.headerContainer}>
					<Typography variant="h2">
						Фильмы
					</Typography>
				</Box>
				<FilmList pageNumber={page} onPage={count} genres={state.genres} />
			</Container>
		</div>
	)
}

export default Films
