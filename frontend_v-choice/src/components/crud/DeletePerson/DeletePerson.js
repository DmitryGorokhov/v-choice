import React from 'react'
import {
	Button,
	Dialog,
	DialogActions,
	DialogContent,
	DialogContentText,
	DialogTitle,
} from '@material-ui/core'
import DeleteIcon from '@material-ui/icons/Delete'

import MyAlerter from '../../atoms/MyAlerter/MyAlerter'

function DeletePerson(props) {
	const [open, setOpen] = React.useState(false);
	const [error, setError] = React.useState(null);
	const [msg, setMsg] = React.useState(null);

	const handleClickOpen = () => {
		setOpen(true);
	};

	const handleClose = () => {
		setOpen(false);
		setError(null);
		setMsg(null);
	};

	const handleSubmit = () => {
		fetch(`https://localhost:5001/api/person/${props.id}`, {
			method: 'DELETE'
		})
			.then(response => {
				if (response.status === 401) {
					setError("Недостаточно прав для выполнения операции");
				}
				if (response.status === 204) {
					setMsg("Успешно удален");
					props.onDelete();
				}
			});
	};

	return (
		<>
			<Button color="secondary" onClick={handleClickOpen}>
				<DeleteIcon />
			</Button>

			<Dialog open={open} onClose={handleClose} aria-labelledby="form-dialog-title">
				<DialogTitle id="form-dialog-title">Удалить знаменитость</DialogTitle>
				<DialogContent>
					<MyAlerter msg={msg} error={error} />
					<DialogContentText>
						Вы действительно хотите удалить данные о знаменитости?
					</DialogContentText>
				</DialogContent>
				<DialogActions>
					<Button onClick={handleClose} color="primary">
						Закрыть
					</Button>
					<Button onClick={handleSubmit} color="primary">
						Удалить
					</Button>
				</DialogActions>
			</Dialog>
		</>
	)
}

export default DeletePerson