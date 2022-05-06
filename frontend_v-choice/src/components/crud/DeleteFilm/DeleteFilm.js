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

function DeleteFilm(props) {
	const [open, setOpen] = React.useState(false);
	const [error, setError] = React.useState(null);
	const [msg, setMsg] = React.useState(null);

	let film = props.film;

	const handleClickOpen = () => {
		setOpen(true);
	};

	const handleClose = () => {
		setOpen(false);
		setError(null);
		setMsg(null);
	};

	const handleSubmit = () => {
		const postURL = `https://localhost:5001/api/film/${film.id}`;
		fetch(postURL, {
			method: 'DELETE'
		})
			.then(response => {
				if (response.status === 401) {
					setError("Недостаточно прав для выполнения операции");
				}
				if (response.status === 204) {
					setMsg("Фильм успешно удален");
					props.onDelete(film);
				}
			});
	};

	return (
		<>
			<Button
				variant="outlined"
				color="secondary"
				className={props.btnStyle}
				onClick={handleClickOpen}
				size="small"
			>
				<DeleteIcon />
				Удалить
			</Button>

			<Dialog open={open} onClose={handleClose} aria-labelledby="form-dialog-title">
				<DialogTitle id="form-dialog-title">Удалить фильм</DialogTitle>
				<DialogContent>
					<MyAlerter msg={msg} error={error} />
					<DialogContentText>
						Вы действительно хотите удалить фильм {film.Title}?
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

export default DeleteFilm
