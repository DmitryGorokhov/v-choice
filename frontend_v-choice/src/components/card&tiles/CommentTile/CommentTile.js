import React, { useContext } from 'react'
import { createStyles, makeStyles, Box, Card, Grid, Typography } from '@material-ui/core'

import UpdateCommentDialog from '../../crud/UpdateCommentDialog/UpdateCommentDialog'
import DeleteComment from '../../crud/DeleteComment/DeleteComment'
import UserContext from '../../../context'

const useStyles = makeStyles((theme) => createStyles({
	card: {
		width: '100%',
		padding: theme.spacing(2, 3),
	},
	container: {
		display: 'flex',
		flexDirection: 'column',
		alignItems: 'center',
	},
	controls: {
		margin: theme.spacing(2, 0, 0),
	},
	controlsContainer: {
		display: 'flex',
		justifyContent: 'center',
	},
	text: {
		fontSize: '18px',
		lineHeight: "150%",
		marginTop: theme.spacing(2),
	},
}));

function CommentTile(props) {
	const classes = useStyles();
	const { user, _ } = useContext(UserContext);

	const dateFormat = (date) => {
		const toDate = new Date(date);
		return `${toDate.getDate()}.${toDate.getMonth() + 1}.${toDate.getFullYear()}`;
	}

	return (
		<Card className={classes.card}>
			<Grid container spacing={2}>
				<Grid item xs={10}>
					<Typography>{props.comment.authorEmail}</Typography>
					<Typography className={classes.text}>{props.comment.text}</Typography>
				</Grid>
				<Grid item xs={2}>
					<Box className={classes.container}>
						<Typography>{dateFormat(props.comment.createdAt)}</Typography>
						{
							user.userName === props.comment.authorEmail
								? <Box className={classes.controlsContainer}>
									<UpdateCommentDialog className={classes.controls} onUpdate={props.onUpdate} comment={props.comment} />
									<DeleteComment className={classes.controls} onDelete={props.onDelete} commentId={props.comment.id} />
								</Box>
								: user.isAdmin
									? <Box className={classes.controlsContainer}>
										<DeleteComment className={classes.controls} onDelete={props.onDelete} commentId={props.comment.id} />
									</Box>
									: null
						}
					</Box>
				</Grid>
			</Grid>
		</Card>
	)
}

export default CommentTile
