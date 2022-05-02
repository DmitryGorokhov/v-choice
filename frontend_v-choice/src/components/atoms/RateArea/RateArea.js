import React, { useContext, useEffect, useState } from 'react'
import { Link } from 'react-router-dom'
import { Box, Button, Typography } from '@material-ui/core'
import { Rating } from '@material-ui/lab'
import ClearIcon from '@material-ui/icons/Clear'
import UserContext from '../../../context'

function RateArea(props) {
	const { user, setUser } = useContext(UserContext);
	const [userRate, setUserRate] = useState(null)

	useEffect(() => {
		if (props.user !== null && props.filmId !== null) {
			fetch(`https://localhost:5001/api/rate/${props.filmId}`)
				.then(response => response.json())
				.then(result => setUserRate(result))
				.catch(_ => _);
		}
	}, [])

	const handleDeleteUserRate = () => {
		const oldValue = userRate.value;
		fetch(`https://localhost:5001/api/rate/${userRate.id}`, {
			method: 'DELETE'
		})
			.then(response => {
				if (response.status === 204) {
					setUserRate(null)
					props.onAction(-oldValue, -1)
				}
			})
	}

	const handleUserRateChanged = (event, newValue) => {
		if (userRate === null) {
			fetch("https://localhost:5001/api/rate", {
				method: 'POST',
				headers: {
					'Content-Type': 'application/json;charset=utf-8'
				},
				body: JSON.stringify({ filmId: props.filmId, value: newValue })
			})
				.then(response => response.json())
				.then(result => {
					setUserRate(result)
					props.onAction(result.value, 1)
				})
		}
		else {
			const oldValue = userRate.value
			const updated = { ...userRate, value: newValue }
			fetch(`https://localhost:5001/api/rate/${userRate.id}`, {
				method: 'PUT',
				headers: {
					'Content-Type': 'application/json;charset=utf-8'
				},
				body: JSON.stringify(updated)
			})
				.then(response => {
					if (response.status === 201) {
						setUserRate(updated)
						props.onAction(newValue - oldValue, 0)
					}
				})
		}
	}

	return (
		<>
			<Typography>Рейтинг фильма:</Typography>
			<Typography>{props.filmRate}</Typography>
			<Box>
				{
					user.userName
						? <>
							<Typography>
								Ваша оценка{userRate === null ? "" : `:${userRate.value}`}
							</Typography>
							<Rating
								value={userRate === null ? null : userRate.value}
								max={10}
								onChange={handleUserRateChanged} />
							<Button variant="primary" onClick={handleDeleteUserRate}>
								<ClearIcon />
							</Button>
						</>
						: <Typography><Link to="/sign-in">Войдите</Link>, чтобы оценить фильм.</Typography>
				}
			</Box>
		</>
	)
}

export default RateArea
