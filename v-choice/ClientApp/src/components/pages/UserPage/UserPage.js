import React, { useState } from 'react'
import { Typography } from '@material-ui/core'

import { NavMenu } from '../../atoms/NavMenu/NavMenu'
import FavoritesList from '../../moleculas/FavoritesList/FavoritesList';

function UserPage() {
	const [userEmail, setUserEmail] = useState(null);

	const loadPage = () => {
		fetch("api/account/isAuthenticated", {
			method: 'POST',
			headers: {
				'Content-Type': 'application/json'
			},
		})
			.then(response => response.json())
			.then(result => {
				result.message === "guest"
					? setUserEmail(null)
					: setUserEmail(result.message)
			});
	}

	loadPage();
	return (
		<div>
			<NavMenu />
			{
				userEmail !== null
					? userEmail === "guest"
						? <Typography variant="h5">
							Авторизируйтесь, чтобы смотреть профиль
							</Typography>
						: <div>
							<Typography variant="h3">{userEmail}</Typography>
							<Typography variant="h5">Мои избранные фильмы</Typography>
							<FavoritesList />
						</div>
					: <Typography>Загрузка...</Typography>
			}
		</div>
	)
}

export default UserPage
