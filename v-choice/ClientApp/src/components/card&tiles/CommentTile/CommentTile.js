import React from 'react'
import { Box, Card, Collapse, Typography } from '@material-ui/core'
import styles from './CommentTile.module.css'

function CommentTile(props) {
	return (
		<div>
			<Card className={styles.card}>
				<Box className={styles.contentContainer}>
					<Typography variant='h6'>
						{props.comment.Text}
					</Typography>
					<Typography className={styles.contentDate}>
						{props.comment.CreatedAt}
					</Typography>
				</Box>
				<Collapse in={props.comment.AuthorId === props.userId}>
					{
						<Box className={styles.controlsContainer}>
							{/* <UpdateCommentDialog />
							<DeleteComment /> */}
						</Box>
					}
				</Collapse>
			</Card>
		</div>
	)
}

export default CommentTile
