import React from 'react'
import { Box, Card, Collapse, Typography } from '@material-ui/core'

import styles from './CommentTile.module.css'
import UpdateCommentDialog from '../../crud/UpdateCommentDialog/UpdateCommentDialog'

function CommentTile(props) {
	const dateFormat = (date) => {
		date = new Date(date);
		return `${date.getDate()}.${date.getMonth() + 1}.${date.getFullYear()}`;
	}
	return (
		<div>
			<Card className={styles.card}>
				<Box className={styles.contentContainer}>
					<Typography variant='h6'>
						{props.comment.Text}
					</Typography>
					<Typography className={styles.contentDate}>
						{dateFormat(props.comment.CreatedAt)}
					</Typography>
				</Box>
				<Collapse in={props.comment.AuthorEmail === props.userEmail}>
					{
						<Box className={styles.controlsContainer}>
							<UpdateCommentDialog filmId={props.comment.FilmId} onUpdateMethod={props.onUpdateMethod} commentId={props.comment.Id} />
							{/* <DeleteComment /> */}
						</Box>
					}
				</Collapse>
			</Card>
		</div>
	)
}

export default CommentTile
