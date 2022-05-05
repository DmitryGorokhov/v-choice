import React, { useState } from 'react'
import { createStyles, makeStyles, Button, Grid, TextField, Typography } from '@material-ui/core'

const useStyles = makeStyles((theme) => createStyles({
	header: {
		margin: theme.spacing(2, 0),
	},
}));

function CommentArea(props) {
	const classes = useStyles();
	const [text, setText] = useState("")

	const handleTextChanged = (event) => {
		setText(event.target.value);
	}

	const handleSubmit = () => {
		fetch('https://localhost:5001/api/comment', {
			method: 'POST',
			headers: {
				'Content-Type': 'application/json;charset=utf-8'
			},
			body: JSON.stringify({ text: text, filmId: props.filmId })
		})
			.then(response => response.json());
		setText("");
		props.onAdd();
	}

	return (
		<>
			<Typography variant='h5' className={classes.header}>Есть мнение? Оставьте комментарий</Typography>
			<Grid container spacing={3}>
				<Grid item xs={11}>
					<TextField
						id="outlined-multiline-static"
						label="Поделитесь: чем вас поразил этот фильм?"
						multiline
						rows={3}
						variant="outlined"
						value={text}
						onChange={handleTextChanged}
						fullWidth
					/>
				</Grid>
				<Grid item xs={1}>
					<Button variant="outlined" onClick={handleSubmit}>Ок</Button>
				</Grid>
			</Grid>
		</>
	)
}

export default CommentArea
