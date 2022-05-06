import React, { useState } from 'react'
import { Button, Dialog, DialogActions, DialogContent, DialogTitle, Typography, } from '@material-ui/core'
import DeleteOutlineOutlinedIcon from '@material-ui/icons/DeleteOutlineOutlined'

function DeleteComment(props) {
	const [open, setOpen] = useState(false);

	const handleClickOpen = () => {
		setOpen(true);
	};

	const handleClose = () => {
		setOpen(false);
	};

	const handleSubmit = () => {
		fetch(`https://localhost:5001/api/comment/${props.commentId}`, {
			method: 'DELETE',
		});
		props.onDelete(props.commentId);
		setOpen(false);
	};

	return (
		<>
			<Button size="small" onClick={handleClickOpen} className={props.className}>
				<DeleteOutlineOutlinedIcon />
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
		</>
	)
}

export default DeleteComment
