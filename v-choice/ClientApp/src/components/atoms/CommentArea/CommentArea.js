import React, { useState } from 'react'
import { Box, Button, TextField, Typography } from '@material-ui/core'
import styles from './CommentArea.module.css'

function CommentArea(props) {
	const [text, setText] = useState('')

	const handleTextChanged = (event) => {
		setText(event.target.value);
	}

	const handleSubmit = () => {
		let comment = {
			Text: text,
			FilmId: props.filmId
		};
		if (props.typeMethod === "create") {
			fetch('/api/comments', {
				method: 'POST',
				headers: {
					'Content-Type': 'application/json;charset=utf-8'
				},
				body: JSON.stringify(comment)
			})
				.then(response => response.json())
				.then(result => props.method(result));
			setText('');
		};
		if (props.typeMethod === "update") {
			comment.Id = props.commentId;
			fetch(`/api/comments/${props.commentId}`, {
				method: 'PUT',
				headers: {
					'Content-Type': 'application/json;charset=utf-8'
				},
				body: JSON.stringify(comment)
			});
			props.method(comment);
			setText('');
		};
	}

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
					value={text}
					onChange={handleTextChanged}
				/>
				<Box>
					<Button variant="outlined" onClick={handleSubmit}>
						Отправить
					</Button>
				</Box>
			</Box>
		</div>
	)
}

export default CommentArea
