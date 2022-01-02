import React, { useEffect, useState } from 'react'
import { createStyles, makeStyles, Box, Typography } from '@material-ui/core'

import { NavMenu } from '../../atoms/NavMenu/NavMenu'
import FavoritesList from '../../moleculas/FavoritesList/FavoritesList'

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
	const [userEmail, setUserEmail] = useState(null);
	const styles = useStyles();

	useEffect(() => {
		fetch("https://localhost:5001/api/account/isAuthenticated", {
			method: 'POST',
			headers: {
				'Content-Type': 'application/json'
			},
		})
			.then(response => response.json())
			.then(result => setUserEmail(result.message));
	}, [])

	return (
		<div>
			<NavMenu />
			<Box className={styles.container}>
				{
					userEmail !== null
						? userEmail === "guest"
							? <Typography variant="h5" className={styles.marginItem}>
								Авторизируйтесь, чтобы просматривать профиль
							</Typography>
							: <Box>
								<Typography variant="h3" className={styles.marginItem}>
									Привет, {userEmail}!
								</Typography>
								<Typography variant="h5" className={styles.marginItem}>
									Вот твои избранные фильмы
								</Typography>
								<FavoritesList />
							</Box>
						: <Typography className={styles.marginItem}>Загрузка...</Typography>
				}
			</Box>
		</div>
	)
}

export default UserPage
