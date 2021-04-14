import React from 'react'
import { Box, Button, TextField, Typography } from '@material-ui/core'
import styles from './CommentArea.module.css'

function CommentArea() {
	return (
		<div>
			<Typography variant='h5' className={styles.header}>
				Есть мнение? Оставьте комментарий
			</Typography>
			<Box className={styles.container}>
				<TextField
					id="outlined-multiline-static"
					label="Поделитесь: чем вас поразил этот фильм?"
					multiline
					rows={4}
					variant="outlined"
					className={styles.textField}
				/>
				<Box>
					<Button variant="outlined" >
						Отправить
					</Button>
				</Box>
			</Box>
		</div>
	)
}

export default CommentArea
