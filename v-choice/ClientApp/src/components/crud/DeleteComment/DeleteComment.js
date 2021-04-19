import React, { useState } from 'react'
import {
	Button,
	Dialog,
	DialogActions,
	DialogContent,
	DialogTitle,
	Typography,
} from '@material-ui/core'

function DeleteComment(props) {
	const [open, setOpen] = useState(false);

	const handleClickOpen = () => {
		setOpen(true);
	};

	const handleClose = () => {
		setOpen(false);
	};

	const handleSubmit = () => {
		fetch(`/api/comments/${props.commentId}`, {
			method: 'DELETE',
			headers: {
				'Content-Type': 'application/json;charset=utf-8'
			},
		});
		props.onDeleteMethod(props.commentId);
		setOpen(false);
	};

	return (
		<div>
			<Button variant="outlined" color="primary" onClick={handleClickOpen}>
				Удалить
			</Button>
			<Dialog open={open} onClose={handleClose} aria-labelledby="form-dialog-title">
				<DialogTitle id="form-dialog-title">Удалить комментарий</DialogTitle>
				<DialogContent>
					<Typography>Вы действительно хотите удалить комментарий?</Typography>
				</DialogContent>
				<DialogActions>
					<Button onClick={handleClose} color="primary">
						Отменить
          			</Button>
					<Button onClick={handleSubmit} color="primary">
						Удалить
          			</Button>
				</DialogActions>
			</Dialog>
		</div>
	)
}

export default DeleteComment
