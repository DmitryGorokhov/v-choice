import React from 'react'
import { Box, makeStyles, createStyles, Typography } from '@material-ui/core'
import FilmList from './FilmList'

const useStyles = makeStyles((theme) => createStyles({
	headerContainer: {
		display: 'flex',
		margin: theme.spacing(2),
		padding: theme.spacing(1),
		justifyContent: 'space-between'
	},
}));

function Films() {
	const classes = useStyles();

	return (
		<Box>
			<Box className={classes.headerContainer}>
				<Typography variant="h2">
					Фильмы
				</Typography>
			</Box>
			<FilmList />
		</Box>
	)
}

export default Films
