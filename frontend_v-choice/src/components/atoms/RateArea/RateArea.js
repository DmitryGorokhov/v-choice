import React, { useContext, useEffect, useState } from 'react'
import { Link } from 'react-router-dom'
import { createStyles, makeStyles, Box, Grid, IconButton, Typography } from '@material-ui/core'
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
		justifyContent: 'right'
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
	const [rate, setRate] = useState(null);

	useEffect(() => {
		if (props.filmId) {
			fetch(`https://localhost:5001/api/rate/${props.filmId}`)
				.then(response => response.json())
				.then(result => {
					if (result && result.rate !== undefined) {
						setRate(result.rate);
					}
				})
				.catch(_ => _);
		}
	}, [])

	const handleDeleteUserRate = () => {
		if (rate) {
			const oldValue = rate.value;
			fetch(`https://localhost:5001/api/rate/${rate.id}`, {
				method: 'DELETE',
			})
				.then(response => {
					if (response.status === 204) {
						setRate(null);
						props.onAction(-oldValue, -1);
					}
				})
				.catch(_ => _);
		}
	}

	const handleUserRateChanged = (_, newValue) => {
		if (rate === null) {
			fetch("https://localhost:5001/api/rate", {
				method: 'POST',
				headers: {
					'Content-Type': 'application/json;charset=utf-8'
				},
				body: JSON.stringify({ filmId: props.filmId, value: newValue })
			})
				.then(response => response.json())
				.then(result => {
					if (result) {
						setRate(result);
						props.onAction(newValue, 1);
					}
				})
				.catch(_ => _);
		}
		else {
			const oldValue = rate.value;
			const updated = { ...rate, value: newValue };

			fetch(`https://localhost:5001/api/rate/${rate.id}`, {
				method: 'PUT',
				headers: {
					'Content-Type': 'application/json;charset=utf-8'
				},
				body: JSON.stringify(updated)
			})
				.then(response => {
					if (response.status === 204) {
						setRate(updated);
						props.onAction(newValue - oldValue, 0);
					}
				})
				.catch(_ => _);
		}
	}

	return (
		<Box className={classes.main}>
			<Grid container>
				<Grid item xs={6}>
					<Box className={classes.container}>
						<Typography className={classes.text}>Рейтинг фильма: {props.filmRate.toFixed(2)}</Typography>
						<Rating value={props.filmRate.toFixed(2)} precision={0.25} max={10} disabled className={classes.stars} readOnly />
					</Box>
				</Grid>
				<Grid item xs={6}>
					<Box className={classes.rightContainer}>
						{
							user.userName
								? <>
									<Typography className={classes.text}>Ваша оценка:</Typography>
									<Rating
										max={10}
										value={rate ? rate.value : 0}
										className={classes.stars}
										onChange={handleUserRateChanged} />
									<IconButton
										aria-label="reset-rate"
										variant="primary"
										size="small"
										onClick={handleDeleteUserRate}
										disabled={rate === null}
									>
										<HighlightOffIcon />
									</IconButton>
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
