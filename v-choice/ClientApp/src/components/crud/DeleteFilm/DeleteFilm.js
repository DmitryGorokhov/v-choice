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
	};

	const handleSubmit = () => {
		const postURL = `api/films/${film.Id}`;
		console.log(postURL);
		fetch(postURL, {
			method: 'DELETE'
		})
			.then(response => {
				if (response.status === 401) {
					setError("Недостаточно прав для выполнения операции");
				}
				if (response.status === 204) {
					setMsg("Фильм успешно удален");
				}
			});
	};

	return (
		<div>
			<Button
				variant="outlined"
				color="secondary"
				className={props.btnStyle}
				onClick={handleClickOpen}
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

export default DeleteFilm
