import React from 'react'
import { createStyles, makeStyles, Avatar, Box, Card, CardContent, Typography } from '@material-ui/core'

const useStyles = makeStyles((theme) => createStyles({
	card: {
		maxWidth: 200,
	},
	content: {
		display: 'flex',
		flexDirection: 'column',
		alignItems: 'center',
	},
}));

function PersonCard(props) {
	const baseURL = 'https://localhost:5001/';
	const classes = useStyles();

	return (
		<Card className={classes.card}>
			<CardContent className={classes.content}>
				<Typography gutterBottom variant="body2" component="div">
					{props.role}
				</Typography>
				<Avatar
					alt={props.fullName}
					src={`${baseURL}${props.photoPath}`}
					style={{ width: 150, height: 150 }}
				/>
				<Typography gutterBottom variant="h6" component="div">
					{props.fullName}
				</Typography>
			</CardContent>
		</Card >
	)
}

export default PersonCard