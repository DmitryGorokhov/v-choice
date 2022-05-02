import React, { useContext } from 'react'
import { createStyles, makeStyles, Box, Typography } from '@material-ui/core'

import FavoritesList from '../../moleculas/FavoritesList/FavoritesList'
import UserContext from '../../../context';

const useStyles = makeStyles((theme) => createStyles({
	marginItem: {
		margin: theme.spacing(2),
	},
	container: {
		margin: theme.spacing(2),
		padding: theme.spacing(1),
	}
}));

function UserPage() {
	const styles = useStyles();
	const { user, setUser } = useContext(UserContext);

	return (
		<Box className={styles.container}>
			{
				user.userName
					? <Box>
						<Typography variant="h3" className={styles.marginItem}>
							Привет, {user.userName}!
						</Typography>
						<Typography variant="h5" className={styles.marginItem}>
							Вот твои избранные фильмы
						</Typography>
						<FavoritesList />
					</Box>
					: <Typography variant="h5" className={styles.marginItem}>
						Авторизируйтесь, чтобы просматривать профиль
					</Typography>
			}
		</Box>
	)
}

export default UserPage
