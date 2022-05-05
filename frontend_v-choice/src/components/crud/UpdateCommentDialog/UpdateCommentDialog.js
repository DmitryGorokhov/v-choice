import React, { useState } from 'react'
import { Button, Dialog, DialogActions, DialogContent, DialogTitle, Grid, TextField } from '@material-ui/core'
import CreateOutlinedIcon from '@material-ui/icons/CreateOutlined'

function UpdateCommentDialog(props) {
	const [open, setOpen] = useState(false);
	const [text, setText] = useState(props.comment.text)

	const handleTextChanged = (event) => {
		setText(event.target.value);
	}

	const handleClickOpen = () => {
		setOpen(true);
	};

	const handleClose = () => {
		setOpen(false);
	};

	const handleSubmit = () => {
		const newComment = { ...props.comment, text: text }
		fetch(`https://localhost:5001/api/comment/${newComment.id}`, {
			method: 'PUT',
			headers: {
				'Content-Type': 'application/json;charset=utf-8'
			},
			body: JSON.stringify(newComment)
		});

		props.onUpdate(newComment);
		setOpen(false);
	}

	return (
		<>
			<Button size="small" onClick={handleClickOpen} className={props.className}>
				<CreateOutlinedIcon />
			</Button>
			<Dialog
				fullWidth
				maxWidth
				open={open}
				onClose={handleClose}
				aria-labelledby="form-dialog-title"
			>
				<DialogTitle id="form-dialog-title">Изменить комментарий</DialogTitle>
				<DialogContent>
					<Grid container spacing={2}>
						<Grid item xs={11}>
							<TextField
								id="outlined-multiline-static"
								label="Поделитесь: чем вас поразил этот фильм?"
								multiline
								rows={4}
								variant="outlined"
								value={text}
								onChange={handleTextChanged}
								fullWidth
							/>
						</Grid>
						<Grid item xs={1}>
							<Button variant="outlined" onClick={handleSubmit}>
								Отправить
							</Button>
						</Grid>
					</Grid>
				</DialogContent>
				<DialogActions>
					<Button onClick={handleClose} color="primary">
						Отменить
					</Button>
				</DialogActions>
			</Dialog>
		</>
	)
}

export default UpdateCommentDialog
