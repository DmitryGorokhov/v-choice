import React from 'react'
import { Box, makeStyles, createStyles, Typography, Container } from '@material-ui/core'
import FilmList from '../../moleculas/FilmList/FilmList'
import { NavMenu } from '../../atoms/NavMenu/NavMenu';

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
		<div>
			<NavMenu />
			<Container>
				<Box className={classes.headerContainer}>
					<Typography variant="h2">
						Фильмы
				</Typography>
				</Box>
				<FilmList />
			</Container>
		</div>
	)
}

export default Films
