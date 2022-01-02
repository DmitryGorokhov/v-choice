import React from 'react'
import { Box, Card, Collapse, Typography } from '@material-ui/core'

import styles from './CommentTile.module.css'
import UpdateCommentDialog from '../../crud/UpdateCommentDialog/UpdateCommentDialog'
import DeleteComment from '../../crud/DeleteComment/DeleteComment'

function CommentTile(props) {
	const dateFormat = (date) => {
		date = new Date(date);
		return `${date.getDate()}.${date.getMonth() + 1}.${date.getFullYear()}`;
	}
	return (
		<div>
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
				<Collapse in={props.comment.AuthorEmail === props.userEmail}>
					{
						<Box className={styles.controlsContainer}>
							<UpdateCommentDialog filmId={props.comment.filmId} onUpdateMethod={props.onUpdateMethod} commentId={props.comment.id} />
							<DeleteComment onDeleteMethod={props.onDeleteMethod} commentId={props.comment.id} />
						</Box>
					}
				</Collapse>
			</Card>
		</div>
	)
}

export default CommentTile
