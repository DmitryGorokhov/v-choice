import React, { useContext, useEffect, useState } from 'react'
import { Link } from 'react-router-dom'
import { createStyles, makeStyles, Box, Button, Grid, Typography } from '@material-ui/core'
import { Rating } from '@material-ui/lab'
import HighlightOffIcon from '@material-ui/icons/HighlightOff'
import UserContext from '../../../context'

const useStyles = makeStyles((theme) => createStyles({
	main: {
		margin: theme.spacing(2, 0),
	},
	container: {
		display: 'flex',
		alignContent: 'center',
	},
	rightContainer: {
		display: 'flex',
		alignContent: 'center',
		justifyContent: 'right',
	},
	text: {
		fontSize: '18px',
		lineHeight: "150%",
	},
	stars: {
		marginLeft: theme.spacing(1),
	}
}));

function RateArea(props) {
	const classes = useStyles();
	const { user, _ } = useContext(UserContext);
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

	const handleUserRateChanged = (_, newValue) => {
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
		<Box className={classes.main}>
			<Grid container>
				<Grid item xs={6}>
					<Box className={classes.container}>
						<Typography className={classes.text}>Рейтинг фильма: {props.filmRate}</Typography>
						<Rating value={props.filmRate} max={10} disabled className={classes.stars} />
					</Box>
				</Grid>
				<Grid item xs={6}>
					<Box className={classes.rightContainer}>
						{
							user.userName
								? <>
									<Typography className={classes.text}>Ваша оценка:</Typography>
									<Rating
										value={userRate === null ? null : userRate.value}
										max={10}
										className={classes.stars}
										onChange={handleUserRateChanged} />
									<Button variant="primary" onClick={handleDeleteUserRate}>
										<HighlightOffIcon />
									</Button>
								</>
								: <Typography><Link to="/sign-in">Авторизируйтесь</Link>, чтобы оценить фильм</Typography>
						}
					</Box>
				</Grid>
			</Grid>
		</Box>
	)
}

export default RateArea
