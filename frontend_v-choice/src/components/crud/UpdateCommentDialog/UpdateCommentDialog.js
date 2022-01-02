import React, { useState } from 'react'
import {
	Button,
	Dialog,
	DialogActions,
	DialogContent,
	DialogTitle,
} from '@material-ui/core'
import CreateIcon from '@material-ui/icons/Create'

import CommentArea from '../../atoms/CommentArea/CommentArea'

function UpdateCommentDialog(props) {
	const [open, setOpen] = useState(false);

	const handleClickOpen = () => {
		setOpen(true);
	};

	const handleClose = () => {
		setOpen(false);
	};

	const onUpdate = (comment) => {
		props.onUpdateMethod(comment);
		setOpen(false);
	}

	return (
		<div>
			<Button variant="outlined" color="primary" onClick={handleClickOpen}>
				<CreateIcon />
				Изменить
			</Button>
			<Dialog
				fullWidth={true}
				maxWidth={false}
				open={open}
				onClose={handleClose}
				aria-labelledby="form-dialog-title"
			>
				<DialogTitle id="form-dialog-title">Изменить комментарий</DialogTitle>
				<DialogContent>
					<CommentArea filmId={props.filmId} typeMethod="update" method={onUpdate} commentId={props.commentId} />
				</DialogContent>
				<DialogActions>
					<Button onClick={handleClose} color="primary">
						Отменить
          			</Button>
				</DialogActions>
			</Dialog>
		</div>
	)
}

export default UpdateCommentDialog
