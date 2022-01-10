import React from 'react'
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
	let { page, count } = useParams();
	console.log(page);
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
				<FilmList pageNumber={page} onPage={count} />
			</Container>
		</div>
	)
}

export default Films
