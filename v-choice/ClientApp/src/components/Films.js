import React from 'react'
import { Box, Button, makeStyles, createStyles, Typography } from '@material-ui/core'
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
				<Button color="primary">
					Добавить новый
				</Button>
			</Box>
			<FilmList type='all' />
		</Box>
	)
}

export default Films
