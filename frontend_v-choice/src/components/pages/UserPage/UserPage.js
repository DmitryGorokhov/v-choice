import React, { useContext } from 'react'
import { Link } from 'react-router-dom'
import { createStyles, makeStyles, Box, Typography } from '@material-ui/core'

import FavoritesList from '../../moleculas/FavoritesList/FavoritesList'
import UserContext from '../../../context';

const useStyles = makeStyles((theme) => createStyles({
	container: {
		margin: theme.spacing(2),
		padding: theme.spacing(1),
	},
	header: {
		margin: theme.spacing(2, 0, 3),
	}
}));

function UserPage() {
	const classes = useStyles();
	const { user, _ } = useContext(UserContext);

	return (
		<Box className={classes.container}>
			{
				user.userName
					? <>
						<Typography variant='h4' className={classes.header}>Привет, {user.userName}!</Typography>
						<FavoritesList />
					</>
					: <Typography variant='subtitle1'>
						<Link to="/sign-in">Войдите</Link>, чтобы просматривать свой профиль
					</Typography>
			}
		</Box>
	)
}

export default UserPage
