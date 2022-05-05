import React, { useContext } from 'react'
import { Box, Card, Typography } from '@material-ui/core'

import styles from './CommentTile.module.css'
import UpdateCommentDialog from '../../crud/UpdateCommentDialog/UpdateCommentDialog'
import DeleteComment from '../../crud/DeleteComment/DeleteComment'
import UserContext from '../../../context'

function CommentTile(props) {
	const { user, _ } = useContext(UserContext);

	const dateFormat = (date) => {
		const toDate = new Date(date);
		return `${toDate.getDate()}.${toDate.getMonth() + 1}.${toDate.getFullYear()}`;
	}

	return (
		<Card className={styles.card}>
			<Box>
				<Box className={styles.dateContainer}>
					<Typography className={styles.contentDate}>
						{dateFormat(props.comment.createdAt)}
					</Typography>
				</Box>
				<Box className={styles.textContainer}>
					<Typography variant='h6' className={styles.contentText}>
						{props.comment.text}
					</Typography>
				</Box>
			</Box>
			{
				user.userName === props.comment.authorEmail
					? <Box className={styles.controlsContainer}>
						<UpdateCommentDialog filmId={props.comment.filmId} onUpdateMethod={props.onUpdateMethod} commentId={props.comment.id} />
						<DeleteComment onDeleteMethod={props.onDeleteMethod} commentId={props.comment.id} />
					</Box>
					: user.isAdmin
						? <Box className={styles.controlsContainer}>
							<DeleteComment onDeleteMethod={props.onDeleteMethod} commentId={props.comment.id} />
						</Box>
						: null
			}
		</Card>
	)
}

export default CommentTile
