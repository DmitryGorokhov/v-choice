import React from 'react'
import {
	Button,
	Dialog,
	DialogActions,
	DialogContent,
	DialogContentText,
	DialogTitle
} from '@material-ui/core';

function DeleteFilm(props) {
	const [open, setOpen] = React.useState(false);
	let film = props.film;
	let onDeleteFilm = props.foo;

	const handleClickOpen = () => {
		setOpen(true);
	};

	const handleClose = () => {
		setOpen(false);
	};

	const handleSubmit = () => {
		const postURL = `api/films/${film.Id}`;
		fetch(postURL, {
			method: 'DELETE'
		});
		onDeleteFilm(film);
		setOpen(false);
	};

	return (
		<div>
			<Button variant="outlined" color="secondary"
				className={props.btnStyle}
				onClick={handleClickOpen}>
				Удалить
			</Button>

			<Dialog open={open} onClose={handleClose} aria-labelledby="form-dialog-title">
				<DialogTitle id="form-dialog-title">Удалить фильм</DialogTitle>
				<DialogContent>
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
